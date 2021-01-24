# Welcome to the translation team!

I am glad to have you here and I hope you will enjoy your time helping LaserGRBL project by translating it into your language.
I know this whole thing may seem a bit hard at first, there is definitely a bit of a learning curve, but below you can find a set of guidelines that should make this process a little bit easier.

## Getting Started

There are some tools that you should definitly have to contribute with translation in a in an effective manner.
- A Git client for interact with github (download source code and submit your translation)
- A software for edit translation file

As Git client I suggest to download Github Desktop from here: https://desktop.github.com

To translate language files I suggest a good tool for editing resx files, called ResEx, which can be downloaded from here:
[ResEx 1.1](https://github.com/arkypita/ResEx/releases/download/v1.1.0.0/install.exe)

**If you have already installed RegEx, please uninstall and use this version (which is able to handle .NET 4.0 resx file)** 

## Clone and translate LaserGRBL
Note: These steps below assume you already downloaded the required/recommended programs.
- Step 1: Login or register to GitHub [(video tutorial)](https://www.youtube.com/watch?v=qxU4QvoMvkE)
- Step 2: Create a fork of this repo ![Fork](https://i.imgur.com/BpugmQo.png)
- Step 3: Clone your fork of this repo using GitHub Desktop![Clone](https://i.imgur.com/1EAOEoM.png)
- Step 4: Start `ResEx`![Open](https://i.imgur.com/9pB3eha.png)
- Step 5: Navigate to the folder where LaserGRBL was cloned to. (Usually ...\Documents\GitHub\LaserGRBL on Windows)
- Step 6: Open the second LaserGRBL directory, then open a `.resx` file, without any suffiexes (like hu_HU etc...)![Select folder](https://i.imgur.com/wqjmbdW.png)
In the example, we will open `SettingsForm.resx` with ResEx
- Step 7: Once opened, change it to your locale![Locale](https://i.imgur.com/hEkJAeX.png)
If your locale is not present, yo can create it this way: ![Locale2](https://i.imgur.com/BRCLW1C.png)
- Step 8: Once your locale is available, you can start translating! Just simply fill the empty spaces on the right in your language. The left side shows the components name, while in the middle there is the english description/text to help you.![Translating](https://i.imgur.com/x9K4g6i.png)
- Step 9: Once done, you can open another `.resx` file without any suffixes (or if you use the filter method, any file showing up). When all is done, you can create a Pull Request (PR) on github.

## Creating a Pull Request (PR)
*(Assuming you already have a GitHub account, and done the prevoius steps)* 
- Step 1: Open Github Desktop
- Step 2: Review your changes, fill the summary, and click `Commit to master` ![Review](https://i.imgur.com/qyLqXsE.png)
- Step 3: Push your changes to your repo ![Push](https://i.imgur.com/vAByaca.png)
- Step 4: Create a PR from your repo![CreatePR](https://i.imgur.com/SGgzlYW.png)![PR](https://i.imgur.com/dxivWsN.png)
