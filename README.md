# LaserGRBL [![Donation](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/pools/c/8cQ1Lo4sRA)
Official website [http://lasergrbl.com](http://lasergrbl.com)

LaserGRBL is a Windows GUI for [GRBL](https://github.com/gnea/grbl/wiki). Unlike other GUI LaserGRBL it is specifically developed for use with laser cutter and engraver. In order to use all of LaserGRBL feature, your engraver must supports laser power modulation through gcode "S" command. LaserGRBL is compatible with [Grbl v0.9](https://github.com/grbl/grbl/) and [Grbl v1.1](https://github.com/gnea/grbl/)

All downloads available at https://github.com/arkypita/LaserGRBL/releases

### Support and Donation

Do you like LaserGRBL? Support development with your donation!

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/pools/c/8cQ1Lo4sRA)

### Existing Features

- GCode file loading with engraving/cutting job preview (with alpha blending for grayscale engraving)
- Image import (jpg, bmp...) with line by line GCode generation (horizontal, vertical, and diagonal).
- Image import (jpg, bmp...) with Vectorization!
- Image import (jpg, bmp...) with 1bit dithering, best result with low power laser
- Vector file import (svg only) [Experimental]
- Different color scheme optimized for different safety glasses
- User defined buttons, power to you!
- Grbl Configuration Import/Export
- Configuration, Alarm and Error codes decoding for Grbl v1.1 (with description tooltip)
- Homing button, Feed Hold button, Resume button and Grbl Reset button
- Job time preview and realtime projection
- Jogging (for any Grbl version)
- Feed overrides (for Grbl > v1.1) with easy-to-use interface
- Support for [WiFi connection via ESP8266 WebSocket](http://lasergrbl.com/en/usage/wifi-with-esp8266/)

### Usage

* [LaserGRBL User Interface](http://lasergrbl.com/usage/user-interface/)
* [Connect to arduino-grbl](http://lasergrbl.com/usage/arduino-connection/)
* [Load G-Code and send to machine](http://lasergrbl.com/usage/load-and-send/)
* [Speed and power overrides](http://lasergrbl.com/usage/overrides/)
* [Jogging](http://lasergrbl.com/usage/jogging/)
* [Custom buttons](http://lasergrbl.com/usage/custom-buttons/)
* [Raster Image Import](http://lasergrbl.com/usage/raster-image-import/)
* [Grayscale conversion parameters](http://lasergrbl.com/usage/raster-image-import/import-parameters/)
* [Line 2 line grayscale conversion](http://lasergrbl.com/usage/raster-image-import/line-to-line-tool/)
* [1bit dithering conversion](http://lasergrbl.com/usage/raster-image-import/dithering-tool/)
* [Image vectorization](http://lasergrbl.com/usage/raster-image-import/vectorization-tool/)

### Development Roadmap

Development status and roadmap can be found here: [Roadmap](https://github.com/arkypita/LaserGRBL/issues/64)

### Missing Features

- Minimal Z axis control (LaserGRBL is for XY machine)

#### Screenshot and videos

[<img src="https://cloud.githubusercontent.com/assets/8782035/23578353/fba95768-00d4-11e7-9357-99c00a30631d.jpg">](https://www.youtube.com/watch?v=Uk2fGoNL3Yk)

![Galeon](https://cloud.githubusercontent.com/assets/8782035/21349915/dba84a5a-c6b4-11e6-965f-a74fd283267a.jpg)

![Raster2Laser](https://cloud.githubusercontent.com/assets/8782035/21425748/34400d46-c84b-11e6-99e5-6eb529a98f8f.jpg)

![Alpha](https://cloud.githubusercontent.com/assets/8782035/21351296/1df460c2-c6bc-11e6-8eee-4612bb7978fa.jpg)

![FinalWork](https://cloud.githubusercontent.com/assets/8782035/21907662/bbe988be-d910-11e6-9bdb-75b6e3404e0a.jpg)

![UserDefinedButtons](https://cloud.githubusercontent.com/assets/8782035/23375844/238e5f70-fd2a-11e6-8826-5ff7743bbea0.jpg)

### Compiling

LaserGRBL is written in C# for .NET Framework 3.5 (or higher) and can be compiled with [SharpDevelop](http://www.icsharpcode.net/opensource/sd/) and of course with [Microsoft Visual Studio](https://www.visualstudio.com) IDE/Compiler

### Licensing

LaserGRBL is free software, released under the [GPLv3 license](https://www.gnu.org/licenses/gpl-3.0.en.html).

### Credits and Contribution

LaserGRBL contains some code from:
- [ColorSlider](https://www.codeproject.com/articles/17395/owner-drawn-trackbar-slider) - Copyright Michal Brylka
- [CsPotrace](https://drawing3d.de/Downloads.aspx) - Copyright Peter Selinger, port by Wolfgang Nagl
- [Bezier2Biarc](https://github.com/domoszlai/bezier2biarc) - Copyright Laszlo
- [websocket-sharp](https://github.com/sta/websocket-sharp) - Copyright sta.blockhead
- [Expression Evaluator](https://github.com/vubiostat/expression.cs) - Copyright Will Gray, Jeremy Roberts
- [GCodeFromSVG](https://github.com/svenhb/GRBL-Plotter) - Copyright Sven Hasemann
- [MS SVG Library](https://archive.codeplex.com/?p=svg) - Microsoft Public License
- [Clipper](http://www.angusj.com/delphi/clipper.php) - Angus Johnson. Copyright © 2010-2014

Thanks to:
- Myself, for italian and english language
- Fernando Luna, sqall123, for spanish translation
- Olivier Salvador, guillaume-rico [#848](https://github.com/arkypita/LaserGRBL/pull/848) for french translation
- Gerd Vogel, for german translation
- Anders Lassen, for danish translation
- Gerson Koppe, for brasilian translation
- Alexey Golovin, Newcomere, AlexeyBond, for russian translation
- Yang Haiqiang, for chinese translation
- 00alkskodi00, for slovak translation [#670](https://github.com/arkypita/LaserGRBL/issues/670)
- ddogman, for hungarian translation [#735](https://github.com/arkypita/LaserGRBL/issues/735)
- Petr Bitnar, for czech translation
- Ozzybanan, for polish translation
- onmaker, for traditional chinese translation [#1066](https://github.com/arkypita/LaserGRBL/pull/1066)
- Nikolaos Ntekas, for Greek translation [#1234](https://github.com/arkypita/LaserGRBL/pull/1234)
- Mrjavaci, for Turkish translation [#1293](https://github.com/arkypita/LaserGRBL/pull/1293)
- Filippo Rivato for code contribution [#305](https://github.com/arkypita/LaserGRBL/pull/305) and again [#1251](https://github.com/arkypita/LaserGRBL/pull/1251)
- Fabio Ferretti for code contribution [#592](https://github.com/arkypita/LaserGRBL/pull/592)
- guillaume-rico for code contribution on Smoothie support
- Tobias Falkner, for code contribution [#937](https://github.com/arkypita/LaserGRBL/pull/937)
- gmmanonymus111, for code contribution [#1032](https://github.com/arkypita/LaserGRBL/pull/1032)
