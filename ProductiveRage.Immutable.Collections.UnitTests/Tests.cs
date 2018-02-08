using Bridge.QUnit;
using Newtonsoft.Json;

namespace ProductiveRage.Immutable.Collections.UnitTests
{
	static class Tests
	{
		private static void Main()
		{
			IntKeyMapTests();
			IntKeyMapSerialisationTests();
			StructKeyMapTests();
			ClassKeyMapTests();
		}

		private static void IntKeyMapTests()
		{
			QUnit.Module("Int-Key Map Tests");

			QUnit.Test("Get returns {Missing} for key not found", assert =>
			{
				var map = Map<int, string>.Empty;
				assert.Equal(map.GetIfPresent(123), Optional<string>.Missing);
			});

			QUnit.Test("Get returns value for key just added", assert =>
			{
				var map = Map<int, string>.Empty
					.AddOrUpdate(123, "abc");
				assert.Equal(map.GetIfPresent(123), "abc");
			});

			QUnit.Test("Get returns {Missing} for key added and then removed", assert =>
			{
				var map = Map<int, string>.Empty
					.AddOrUpdate(123, "abc")
					.RemoveIfPresent(123);
				assert.Equal(map.GetIfPresent(123), Optional<string>.Missing);
			});

			QUnit.Test("Nothing happens if key removed that does not exist", assert =>
			{
				var map = Map<int, string>.Empty
					.RemoveIfPresent(123);
				assert.Ok(true);
			});

			QUnit.Test("Setting key that already exists overwrites existing data", assert =>
			{
				var map = Map<int, string>.Empty
					.AddOrUpdate(123, "abc")
					.AddOrUpdate(123, "xyz");
				assert.Equal(map.GetIfPresent(123), "xyz");
				assert.Equal(map.Count, 1);
			});
		}

		private static void StructKeyMapTests()
		{
			QUnit.Module("Struct-Key Map Tests");

			QUnit.Test("Get returns {Missing} for key not found", assert =>
			{
				var map = Map<MyKey, string>.Empty;
				assert.Equal(map.GetIfPresent(new MyKey(123)), Optional<string>.Missing);
			});

			QUnit.Test("Get returns value for key just added", assert =>
			{
				var map = Map<MyKey, string>.Empty
					.AddOrUpdate(new MyKey(123), "abc");
				assert.Equal(map.GetIfPresent(new MyKey(123)), "abc");
			});

			QUnit.Test("Get returns {Missing} for key added and then removed", assert =>
			{
				var map = Map<MyKey, string>.Empty
					.AddOrUpdate(new MyKey(123), "abc")
					.RemoveIfPresent(new MyKey(123));
				assert.Equal(map.GetIfPresent(new MyKey(123)), Optional<string>.Missing);
			});

			QUnit.Test("Nothing happens if key removed that does not exist", assert =>
			{
				var map = Map<MyKey, string>.Empty
					.RemoveIfPresent(new MyKey(123));
				assert.Ok(true);
			});

			QUnit.Test("Setting key that already exists overwrites existing data", assert =>
			{
				var map = Map<MyKey, string>.Empty
					.AddOrUpdate(new MyKey(123), "abc")
					.AddOrUpdate(new MyKey(123), "xyz");
				assert.Equal(map.GetIfPresent(new MyKey(123)), "xyz");
				assert.Equal(map.Count, 1);
			});
		}

		private static void ClassKeyMapTests()
		{
			QUnit.Module("Class-Key Map Tests");

			QUnit.Test("Get returns {Missing} for key not found", assert =>
			{
				var map = Map<MyCompositeId, string>.Empty;
				assert.Equal(map.GetIfPresent(new MyCompositeId(123, "test")), Optional<string>.Missing);
			});

			QUnit.Test("Get returns value for key just added", assert =>
			{
				var map = Map<MyCompositeId, string>.Empty
					.AddOrUpdate(new MyCompositeId(123, "test"), "abc");
				assert.Equal(map.GetIfPresent(new MyCompositeId(123, "test")), "abc");
			});

			QUnit.Test("Get returns {Missing} for key added and then removed", assert =>
			{
				var map = Map<MyCompositeId, string>.Empty
					.AddOrUpdate(new MyCompositeId(123, "test"), "abc")
					.RemoveIfPresent(new MyCompositeId(123, "test"));
				assert.Equal(map.GetIfPresent(new MyCompositeId(123, "test")), Optional<string>.Missing);
			});

			QUnit.Test("Nothing happens if key removed that does not exist", assert =>
			{
				var map = Map<MyCompositeId, string>.Empty
					.RemoveIfPresent(new MyCompositeId(123, "test"));
				assert.Ok(true);
			});

			QUnit.Test("Setting key that already exists overwrites existing data", assert =>
			{
				var map = Map<MyCompositeId, string>.Empty
					.AddOrUpdate(new MyCompositeId(123, "test"), "abc")
					.AddOrUpdate(new MyCompositeId(123, "test"), "xyz");
				assert.Equal(map.GetIfPresent(new MyCompositeId(123, "test")), "xyz");
				assert.Equal(map.Count, 1);
			});
		}

		private static void IntKeyMapSerialisationTests()
		{
			QUnit.Module("Int-Key Map Serialisation Tests");

			QUnit.Test("Single item serialised", assert =>
			{
				var map = Map<int, string>.Empty.AddOrUpdate(123, "abc");
				assert.Equal(
					JsonConvert.SerializeObject(map),
					"[{\"Key\":123,\"Value\":\"abc\"}]"
				);
			});

			QUnit.Test("Single item deserialised", assert =>
			{
				var json = "[{\"Key\":123,\"Value\":\"abc\"}]";
				var map = JsonConvert.DeserializeObject<Map<int, string>>(json);
				assert.Equal(map.Count, 1);
				assert.Equal(map.GetIfPresent(123), "abc");
			});
		}

		private struct MyKey
		{
			public MyKey(int value) { Value = value; }
			public int Value { get; }
		}

		private sealed class MyCompositeId
		{
			public MyCompositeId(int id, string type)
			{
				Id = id;
				Type = type;
			}
			public int Id { get; }
			public string Type { get; }
			public override bool Equals(object o)
			{
				var otherId = o as MyCompositeId;
				return (otherId != null) && (otherId.Id == Id) && (otherId.Type == Type);
			}
			public override int GetHashCode() => Id.GetHashCode() ^ (Type ?? "").GetHashCode();
		}
	}
}
