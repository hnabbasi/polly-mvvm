# Polly-MVVM
Using Polly in your Xamarin.Forms MVVM app.

[Polly]("https://github.com/App-vNext/Polly")

Polly has been around for a while and is a great tool for making your app more responsive and have a strategy around transient faults i.e. dropped network connection, weak transmission resulting in errors, etc. Polly has been document extensively on their page linked above, but in this session, I will demonstrate how to simplify the use of Polly in your MVVM app. You can build from here and make the strategies as complex as needed.

# Architecture
I am going to structure my code in several layers/nodes/tiers however you want to call them. This will keep the code decoupled based on behavior. I will have the following layers,


# Http Client
We will use the managed `HttpClient` for this demo. But since we can swap the implementation of the client for numerous reasons, I am going to keep the client out of my main service using `IHttpClient` abstraction.
