# Chaos Mod for Lethal Company
This mod for Lethal Company is inspired by the [Chaos Mod for GTA 5](https://www.gta5-mods.com/scripts/chaos-mod-v-beta).  
For those who don't know the GTA 5 Chaos Mod, this mod will run random "effects" every 30 seconds (time is configurable).  
These effects can either help you with your game or make it harder to play.  
If you are a Twitch Streamer you can make it even harder by allowing your chat to vote on the effects that you will get.

## Keybinds
This mod has 3 keybinds you can use in case things aren't working like they're supposed to.  
By using Ctrl + P you can restart the Chaos Mod, this can be useful in the case that the mod is not responding anymore or the Twitch Chat breaks for example.  
The keybind Ctrl + O will allow you to turn off the mod, the mod will restart automatically again in the next round though.  
If you want the mod to start again after turning it off you can press Ctrl + P again.  
If the Twitch Chat isn't being handled properly anymore by the mod you can try to reset the token by pressing Ctrl + I, this will open a webpage to authenticate with the Twitch API.

## Twitch Setup
To watch a tutorial video instead click [here](https://youtu.be/9MZd-9G_0_E).  
By opening the config with the name SliceCraft.ChaosMod you can change some of the settings.  
If you want to use Twitch chat voting you need to change the activator to `twitch`  
You can choose between having the effects show up on your screen or in the Twitch chat by changing the `OptionsShowcase` setting.  
By default this setting is set to `onscreen` but if you want it in your chat you can change this to `chatmessage`.  
Additionally if you want to change how often an effect is picked you can change the `TimeBetweenEffects` setting.  
I wouldn't recommend changing the `DelayBeforeSpawn` but you can if you want to.  
Then you just boot into the game, when creating a lobby a browser window should open at `http://localhost:8000`, if it doesn't you can open it manually or click [here](http://localhost:8000).  
Authenticate the Chaos Mod through Twitch and your Twitch chat should be able to vote.  
If you have any issues with the Twitch system you can press Ctrl + I to reopen the window, if that doesn't open the window you will have to go to `http://localhost:8000` manually or click [here](http://localhost:8000).  
This should refresh everything and make it work again.  
Having to authenticat the Chaos Mod should only have to happen once, after that it should recognize the mod automatically.

## Important for Twitch streamers!!!
When authenticating it's possible that the token that will be used for communicating with the chat is temporarily visible in the url bar.  
When playing with this mod you should make sure that your stream can't see the website open which you can do by using a window or game capture so only your game is captured.  

## I accidentally leaked my token, what do I do
If you accidentally showed the token to your chat it's important that you disconnect the mod from your Twitch account by going to [https://www.twitch.tv/settings/connections](https://www.twitch.tv/settings/connections).  
Scroll down to `Other Connections` and press `Disconnect` on the connection called `Lethal Company Chaos Mod`.  
Now no-one should be able to use the leaked token, the next time you play with the mod you do need to authorize the mod again though.

## All Effects
Attractive Player: Make all enemies go to your location  
Damage Roulette: Do a random amount of damage or potentially blow up  
Disable Controles: Disables Controls  
Drop Everything: Drops Everything  
Drop *: Drops the specified item  
Drop Scrap: Drops 2 items of scrap  
Enemies Vote To Leave: The enemies told the ship to leave  
Extra Day: You get an extra day of collecting scrap  
Faster Effects: Effects start twice as fast  
Full Health: You get healed to full health  
Heal: You get partially healed  
Infinite Sprint: You get infinite sprint for a short while  
Invinicibility: You become temporarily invincible  
Invisibile Enemies: Enemies become invisible to you  
No Stamina: Your stamina is depleted temporarily  
One Hit Explosions: If you take the slightest bit of damage you explode  
Random Outfit: You equip a random suit  
Random Teleport: You get teleported to a random location inside the factory  
Remove Holding Items: Your items either disappear or get dropped  
Spawn *: Spawn the specified enemy  
Spinning Enemies: All enemies start spinning around  
Super Jump: Get the ability to jump really high  
Teleport To Heaven: Teleport To Heaven  
Teleport To Ship: Teleport To Ship  
Unlock Unlockable: Unlock a random upgrade  
U Turn: Do a U Turn  
Warp Speed: Get a lot of speed  
YIPPEEEE: Spawn 10 Hoarding Bugs

## Contributing
You are allowed to fork, edit and redistribute this repository.  
This repository is not made public with the intent of getting contributors to work on the mod.  
If you want to contribute feel free to make changes and create a pull request but I can't guarantee if your change will be implemented.  
The configs are made to fit my setup and may not work on your setup.