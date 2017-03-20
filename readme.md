Source for Astonishing Airship Adventures, for the Touchomatic arcade game machine.
----------------------------------------------------------------------------------
For more information about this game, check out our paper at DIS 2017, 
Marshall, Joe, Tennent, Paul. 2017, Touchomatic: Interpersonal Touch Gaming In The Wild, in Proceedings ACM Designing Interactive Systems 2017, ACM New York,NY

The game itself builds on Unity 5.5 or later. You need to build and run touchserver (python app) at the same time to handle the connection with the arduino.

The touchomatic subfolder folder has the arduino code for the touchomatic sensing board. It runs on an arduino uno. 
There is also an SVG file which we used for the lasercut panel to put the sensing handles in.

To build the board, you need two sensor handles (we used BMX bike 'stunt pegs').
Each handle is connected to two arduino analog pins, one direct, and one via a 1 meg resistor. 

