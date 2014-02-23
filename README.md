etcetera
========

.Net client for [etcd](https://github.com/coreos/etcd) - a highly-available key value store for shared configuration and service discovery.

#Getting started

## Initialize

Create the client to work with assuming that you installed etcd on localhost:

```
IEtcdClient client = new EtcdClient(new Uri("http://localhost:4001/v2/keys/"));
```

## Set a key

```
var test = client.Set("test/one", "1"); 
```

## Get a key value
```
var test = client.Get("test/one", true); 
```

## Watch for key changes
```
client.Watch("test/one", FollowUp); 
```

Followup is the callback function:

```
private static void FollowUp(EtcdResponse obj)
{
	// do the thing  
}
```

# Other features

To be done...
