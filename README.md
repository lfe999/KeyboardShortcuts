# Virt-A-Mate Keyboard Shortcuts

Lets you bind keyboard / joystick bindings to many commands in Virt-A-Mate

## Installing

Requires VaM 1.19 or newer.

Download `LFE.KeyboardShortcuts.(version).var` from [Releases](https://github.com/lfe999/KeyboardShortcuts/releases)

Save the `.var` file in the `(VAM_ROOT)\AddonPackages`.

If you have VaM running already, click on the *Main UI > File (Open/Save) > Rescan Add-on Packages* button so that this plugin shows up.

## Quickstart

### Add the plugin to your scene

- Click `Add Plugin` on either an atom or the `Session Plugins` tag from within VaM.
- Click `Select File...` and choose `LFE.KeyboardShortcuts.(version)` on the left.
- Choose the `ADDME.cslist` file

### Configure your keyboard shortcuts

Click `Open Custom UI` on the KeyboardShortcuts plugin.

The dropdowns at the top will let you filter the keyboard shortcuts by category and subcategory.

### Recording a shortcut

To record a new keyboard shortcut, find the command you would like to modify and click the grey button to the right of the description.

Then press the key combination that you would like to use to trigger that command.

Some joypad axis will work but it is limited by the setup of the Unity engine itself.  If it works for you then horray!  If a command has some sort of "Increase" or "Decrease", then this program tries its best to map one direction of an axis to the increase side and one to the decrease side based on the context.

### Clearing a shortcut

Click the selected shortcut grey button.

Hit the `ESC` key

### Saving your shortcuts

Anytime you record or clear a shortcut, the settings are automatically saved in the `(VAM_ROOT)\Saves\lfe_keyboardshortcuts.json` file

### Resetting all shortcuts

Delete the file `(VAM_ROOT)\Saves\lfe_keyboardshortcuts.json` in order to set all shortcuts to their default.

## Command List

### Global Actions

These are commands that will likely apply no matter what atoms are in the scene.

| Command | Notes |
| ------- | ----------- |
Animation Speed > Increase | 
Animation Speed > Decrease | 
Atom > Select Next | Select the next atom alphabetically even if it is hidden
Atom > Select Prev | Select the previous atom alphabetically even if it is hidden
Atom > Select Next Visible | Select the next visible atom alphabetically
Atom > Select Prev Visible | Select the previous visible atom alphabetically
Camera > Move > `Direction` | Move the main camera in the direction that you want
Camera > Look > `Direction` | Rotater the main camera in the direction that you want
Error Log > Toggle | 
Message Log > Toggle | 
Field of View > Increase | Increase the FOV by 10
Field of View > Decrease | Decrease the FOV by 10
Freeze Animation > Toggle | 
Freeze Animation > On |
Freeze Animation > Off | 
Mirror Reflections > Toggle | 
MSAA Level > Increase | Choose the next highest MSAA level. Does nothing if already at the lowest.
MSAA Level > Decrease | Choose the next lowest MSAA level. Does nothing if already at the lowest.
Performance Monitor > Toggle | 
Pixel Light Count > Increase | 
Pixel Light Count > Decrease |
Play/Edit > Set To Edit | Turn on Edit Mode
Play/Edit > Set To Play | Turn on Play Mode
Play/Edit > Toggle | Toggle Edit/Play Mode
Rescan Add-on Packages |
Hard Reset |
Scene > New Scene |
Scene > Open Scene |
Scene > Save Scene |
Screen Shot > Mode > Enable |
Soft Body Physics > Toggle |
Time Scale > Increase | Increase time scale by 0.10 - holding `SHIFT` makes this 0.5
Time Scale > Decrease | Decrease time scale by 0.10 - holding `SHIFT` makes this 0.5
World Scale > Increase | Increase world scale by 0.00025 - holding `SHIFT` makes this 0.001
World Scale > Decrease | Decrease world scale by 0.00025 - holding `SHIFT` makes this 0.001
Add > `Atom Type` > `Atom Name` | Add the atom of the given type to the scene 

### Selected Atom Actions

These are commands that will be run against a selected atom.  They do nothing if no atom is selected.

| Command | Notes |
| ------- | ----------- |
Delete | Delete the selected atom
Hide > Toggle | Toggle the atom hidden or shown
Show UI > `Tab Name` | Show the selected `Tab Name`.  If that tab name does not exist on the selected atom, this does nothing.
Position > `[XYZ]` > Interpolate 0 - 1 | Set the `AXIS` of the atom to something between 0 and 1.  Probably only useful when assigning a joypad axis.
Position > `[XYZ]` > Increase / Decrease Small | Increase or decrease the given `AXIS` position by a maximum of 0.5 units per second
Position > `[XYZ]` > Increase / Decrease Medium | Increase or decrease the given `AXIS` position by a maximum of 2.0 units per second
Position > `[XYZ]` > Increase / Decrease Large | Increase or decrease the given `AXIS` position by a maximum of 5.0 units per second
Rotation > `[XYZ]` > Increase / Decrease Small | Increase or decrease the given `AXIS` rotation by a maximum of 0.25 rotations per second
Rotation > `[XYZ]` > Increase / Decrease Medium | Increase or decrease the given `AXIS` rotation by a maximum of 0.5 rotations per second
Rotation > `[XYZ]` > Increase / Decrease Large | Increase or decrease the given `AXIS` rotation by a maximum of 2.0 rotations per second

### Specific Atom Actions

These are commands that will be run against the atom with the selected `ID`, EVEN IF IT IS NOT SELECTED.

This means that a command to `ShowUI >  Plugins` will first, select the atom, and then run your selected command.

Additionally, commands to target specific **free controllers** and **plugins** can be 

| Command | Notes |
| ------- | ----------- |
see "Selected Atom Commands" |

### Targeting Plugin Commands

Any `Params` that a plugin exposes can be targeted.

In order to trigger a plugin `Params` with a keybinding:
- select the specific atom that has the plugin installed in the left hand dropdown
- select the specific plugin in the right hand dropdown
- set keybinding for the dynamically generated commands available

| Commands | Notes |
| ------- | ----------- |
`Plugin` > Show UI | open the plugins configuration UI
`Boolean Param` > Toggle | Toggle the `Boolean Param` on or off for the plugin
`Boolean Param` > On | Set the `Boolean Param` On for the plugin
`Boolean Param` > Off | Set the `Boolean Param` Off for the plugin
`Action Param` > Call | Trigger the custom `Action Param` for the plugin
`Float Param` > +0.01 | Increase the float param by a maximum of 0.01 (if joypad axis is bound, increased/decreased amount is interpolated)
`Float Param` > -0.01 | 
`Float Param` > +0.10 | 
`Float Param` > -0.10 | 
`Float Param` > +1.00 | 
`Float Param` > -1.00 | 
`String Chooser Param` > Next | Select the next item in for the given `String Chooser Param` of the plugin
`String Chooser Param` > Prev | Select the previous item in for the given `String Chooser Param` of the plugin
