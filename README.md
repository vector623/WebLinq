# WebLinq

this class library is meant to aid functional developers interacting with online web apis.  c#'s standard enumerators
sometimes don't provide enough functionality for web requests, where you often don't know how many subsequent calls to
enumerate through until the first request has returned.  sometimes, as in the case of amazon's marketplace web service 
(2013 version, which is latest to date), you won't know how many subsequent pages of results need to be pulled until 
you're done.

## enumerators

initially adding the AggregateUntil() method, which will accept a seed value (which should be a page result), a func()
informing the method how to pull subsequent requests, as well as another func() informing the method when to stop.

because i usually won't know how many subsequent web calls are required, i will setup a very large number range to loop
through, using Enumerator.Range().  however, i need a way to cancel out of additional iterations of the web request 
after i'm done pulling data. 

so i wrote this so that i could setup iterations of web calls using C#'s linq, but i needed a way to tell linq when to 
 stop looping.  while C#'s LINQ has a reduce() equivalent (aggregate), it is incompatible with the linq takewhile()
method.

## todo
add a console project as a demo and include some mock functions to imitate web requests to the google adwords api and
the amazon marketplace web service api

## example
https://gist.github.com/vector623/9f087f253653f1dd08ed793f1c7ec26f
