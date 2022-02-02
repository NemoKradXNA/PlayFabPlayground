## PlayFab Playground
A friend of mine was telling me about [PlayFab](https://playfab.com/) a Microsoft Azure backend platform to support game development, a much better description for this is provided by Microsoft [here](https://docs.microsoft.com/en-us/gaming/playfab/what-is-playfab).

It has support for Multiplayer services, Chat, LiveOps and loads more. 

So, I signed up for a free plan account, this is sort of a developer mode, so it will cost me nothing to get up to speed with it, but should I want to release a game into the wild using it, there are a number of other plans too. You can check them out in more detail [here](https://playfab.com/pricing/).

The free plan I am using gives me the following:-

Development Mode, Up to 10 Titles with up to 100k users per title.
Try out Multiplayer Server* hosting with up to 750 free compute hours.
Includes up to 10k total minutes of PlayFab Party* Connectivity and Voice.
First 150k Requests Free up to 1 MBs.

Which is pretty cool for free right!

### What am I doing with this Repo?
Well, my idea is to use this as a bit of a playground for me to get my head around PlayFab and how it works. I am hoping to document how I am trying to work with the API and use it in my own projects. The documentation has a load of guides for [platform specific integration](https://docs.microsoft.com/en-us/gaming/playfab/features/authentication/platform-specific-authentication/). I found it quite Unity and Web heavy if I am honest, so, my first attempt to integrate into PlayFab is going to be with [MonoGame](https://www.monogame.net/)

### My First Project
Once you have signed up to PlayFab, you will have access to your studio, here you can register your game title.
![image](https://user-images.githubusercontent.com/2579338/152052384-8e841b7d-fa48-49f6-bbfa-c68726e222a3.png)
As you can see I have created a game called Shooter already, but to create another is super simple.
![image](https://user-images.githubusercontent.com/2579338/152052557-2916d8d0-0a3e-4900-a07d-5c8f90f51d52.png)
Once set up you will see you get an ID on your game's tile on your dashboard, in the first image, you can see I have deleted mine. You will need this when you start to talk to the PlayFab API.

This first project is going to be a super simple shootem up, very basic, for me to get into the PlayFab API and see what I can do with it. Ill be posting my progress as I go here, using this repo as a bit of a blog :)

[Next ->](/LogInWith.md)
