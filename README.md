# Graphics-Lab

## File syntax conventions:
1. Values inside CSV files must be separated by comma characters.
2. CSV files for tong and gripper force must have only one column pertaining to the force value.
3. CSV files for position must have 3 columns pertaining to the X, Y and Z values respectively, where Z is the up-vector.
4. CSV files for joint angles must have 7 columns pertaining to the joints from 0 to 6 in the same order.


## General information (implementation in Unity):
1. Since the Y axis is the up-vector in Unity, the scripts use the Z values from the CSV files as Y and vice versa. Similar approach is used for rotations and angles.
