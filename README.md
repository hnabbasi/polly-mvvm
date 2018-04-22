# Polly-MVVM

Blog: //coming soon to intelliAbb.com

[Polly](https://github.com/App-vNext/Polly) has been around for a while and is a great tool for making your app more responsive and have a strategy around transient faults i.e. dropped network connection, weak transmission resulting in errors, etc. Polly has been document extensively on their page linked above, but in this session, I will demonstrate how to simplify the use of Polly in your MVVM app. You can build from here and make the strategies as complex as needed.

> Polly works with any .NET application. Which means that you can apply these resilience policies to your .NET API layer as well.

# Architecture
I am going to structure my code in several layers/nodes/tiers however you want to call them. This will keep the code decoupled based on behavior. I will have the following layers,

- Models 
- Common
- Services
- Core
- iOS
- Android

In my opinion, the best framework to write MVVM Xamarin.Forms is **Prism.Forms**. It provides services to handle navigation, dependency injection, events, etc. So I will be using **Prism.Forms** for this demo.

# Components
These are the main components of the app that make use of **Polly** in a suitable manner possible.

### IHttpClient
We will use the mono managed `HttpClient` for this demo. But since we can swap the implementation of the client for numerous reasons, I am going to keep the client out of my main service using `IHttpClient` abstraction.

### INetworkService
This service will handle our **Polly** policies. I want to keep all my policies in one place so that I can easily manage them and test them.

### IApiService
This service will handle all API calls. We can choose to use **Polly** policies or not in this service. The developer has the freedom to call APIs as he needs because not all calls need a policy e.g. POST that can cause duplications or errors. All my functional services will inject `IApiService` and will provide APIs to make calls.

# Resilience Policies
For this demo, I will implement `Retry` and `WaitAndRetry` policies. But use these as a starting point and build on top to get more complex policies for your app.

# Scope
**Polly** offers a lot of cool ways to make your app's network resilience awesome. In this demo, I will cover how to use **Polly** in a MVVM-friendly manner that lets you test your service code independently. I will also cover some policies in this demo, and some more in my future demos.

### In This Demo
- Using **Polly** in a service
- Inject policies service
- `Retry` policy
- `WaitAndRetry` policy

### Future Demos
- PolicyRegistry
- Combined policies
- Circuit Breaker
- Cache 
- Fallback

