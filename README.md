# (Bridge.NET) ProductiveRage.Immutable.Collections
The [ProductiveRage.Immutable/Bridge.Immutable](https://github.com/ProductiveRage/Bridge.Immutable) library makes it easier to create with and work with imutable classes in [Bridge.NET](http://bridge.net/) and introduced **Optional&lt;T&gt;** and **NonNullList&lt;T&gt;** types but there are other immutable collection types that are useful once you start writing writing immutable-by-default data structures.

Rather than implement any more myself, this library uses [Facebook's Immutable](https://facebook.github.io/immutable-js/) JavaScript library but puts a more C#-style interface on top, where appropriate (and returns **Optional&lt;T&gt;** value from functions that may or may not return a value, such as the "GetIfPresent" function on the **Map** class).

This library will not necessarily expose *all* of the Facebook library's functionality or types, I will be adding data structures as I find them useful and trying to ensure that their interfaces are consistent

## Map&lt;TKey, TValue&gt;

This is a dictionary-like struture that has the following interface:

	uint Count { get; }
	bool Contains(TKey key);
	Optional<TValue> GetIfPresent(TKey key);
	Map<TKey, TValue> AddOrUpdate(TKey key, TValue value);
	Map<TKey, TValue> RemoveIfPresent(TKey key);
	
(as well as implementing **IEnumerable&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;**).

This structure may be serialised/deserialised using Json.NET (using the .NET build of the library on the server if passing data from an API to a Bridge application or using the Bridge build of Json.NET).