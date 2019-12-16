# Graphics-Lab

## How to use
1. The camera can be changed using the dropdown menu in the top left corner.
2. A point data file for the demonstration must be uploaded first before setting cnstraints.
3. The joint angles can be uploaded using a CSV file. The time scroll and the play button is located at the bottom.
4. Timestamps from ROS can be uploaded using a CSV file.
5. The demonstration and Playback force files can be uploaded using the appropriate buttons and CSV files.
6. The force and joint angle graphs can be toggled using the buttons located at the top right corner.
7. To update the graph with the latest values after uploading the files, choose a different option in the dropdown menu.
8. Animation speed of the robot can be controlled using the slider for it.
9. The postion of the stationary camera can be updated using the text fields. The camera rotates automatically to face the tip of the end effector. This option is to mimic the camera positioned in the real world for the demonstration.
10. Before adding a constraint, make sure the current constraint is set to the appropriate constraint that is intended to be added.
11. The dropdown menu for the list of added constraints updates when a new one is added and the selected constraint can be removed using the 'remove' buutton.
12. Constraint information can be updated using the text fields and then added using the button, or can be uploaded as a CSV file which adds it automatically.
13. The video can be uploaded using the browse button. The time scroll and play button is located below the video screen. The video screen can be moved around by clicking on it and then repositioning it.


## File syntax conventions:
1. Values inside CSV files must be separated by comma characters.
2. CSV files for tong and gripper force must have only one column pertaining to the force value.
3. CSV files for position must have 3 columns pertaining to the X, Y and Z values respectively, where Z is the up-vector.
4. CSV files for joint angles must have 7 columns pertaining to the joints from 0 to 6 in the same order.
5. CSV files for joint angles must be a single file that includes all joint angles from start to end of demonstration.
6. CSV files for constraint information must have the following in the same order and in a single row: wx, wy, wz, dx, dy, dz, radius. 'radius' value must be 0 if constraint is not axial.
7. CSV files for timestamps must have the following ROS time values in the same order and in a single row: safe compliant positioning, safe compliant replay 1, constraint interaction, safe compliant replay 2, end.


## General information (implementation in Unity):
1. Since the Y axis is the up-vector in Unity and Z is the up-vector in the demonstration, the scripts use the Z values from the CSV files as Y and vice versa. Similar approach is used for rotations and angles. (Rotations can get messy and would probably require time to understand the implementation)
2. Windows executable file uses a custom file browser which is included using SFB in the assembly reference.
3. Changing OS platforms would require changes to the file browser code and the message box code.
4. Some joints in the Sawyer model uses anti-clockwise as positive direction while others use clockwise as positive.
5. To implement the rotations correctly, the Sawyer model has child gameobjects ("Body") in some joint objects to reset the rotations to zero for each joint.