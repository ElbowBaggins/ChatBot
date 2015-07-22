# ChatBot
This is the 'core' bit of ChatBot. 
All it does by itself is join channels and idle, but it exposes an interface called 'IChatBotResponder' that you can implement to get it to respond to certain stimuli. There's also an IChatBotConfig interface that you can implement to get configuration from the user or a file or whatever you want really. It uses Meebey.SmartIrc4Net on the backend. Comments are a bit spartan, sorry about that. I wrote this for my personal amusement and evidently I have a thing for intentionally confusing myself.
