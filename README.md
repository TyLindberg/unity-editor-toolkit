Unity Editor Toolkit
====================

A set of helpful Unity editor tools to improve workflow and efficiency

Usage
=====

The Unity Editor Toolkit exposes helpful commands and keyboard shortcuts to make certain operations easier and faster within Unity.

## Snapping

### Snap to Ground (Hotkey: `G`)
`Snap to Ground` allows you to snap an object to rest on the ground below it. If the object has no mesh then it will snap the object root directly to the ground. If the object does have a mesh, then it will snap the lowest vertex to the ground.

*NOTE: Currently this requires the ground to have a collider on it.*

### Snap to Zero (Hotkey: `.`)
`Snap to Zero` is a useful command for snapping an object to the origin after creating it. It has two bits of functionality that are useful. If the object's y-component of local position is not zero, then the first invocation of `Snap to Zero` will snap the object to the xz-plane in local space. If the y component of the object's local position is already 0, then `Snap to Zero` will snap the object to the origin of local space. If the object has no parent it will snap in world space instead.

## Scene View

### Scene Gizmo
These next commands add hotkeys that replicate functionality of the "Scene Gizmo" found in the upper right corner of the Scene View. All commands will function on the last active scene view.

#### Orthographic Toggle (Hotkey: `3`)

#### Right View (Hotkey: `4`)

#### Left View (Hotkey: `SHIFT+4`)

#### Top View (Hotkey: `5`)

#### Bottom View (Hotkey: `SHIFT+5`)

#### Front View (Hotkey: `6`)

#### Back View (Hotkey: `SHIFT+6`)

### Shading Mode
The following commands change the shading mode on the last active scene. For example, this can be useful when wanted to quickly switch between Shaded and Wireframe mode with no mouse movement.

#### Shaded Mode (Hotkey: `7`)

#### Wireframe Mode (Hotkey: `8`)

#### Shaded Wireframe Mode (Hotkey: `9`)

Key Bindings
============

All current key bindings are experimental. If you want to change any of them, you can do so by manually editing the "Key Bindings" section of `Constants.cs`.

Importing to Unity
==================

To import the Unity Editor Toolkit into your Unity project, all you need to do is copy the `UnityEditorToolkit` directory of this repo into the `Assets` directory of your project. You now have access to all Unity Editor Toolkit commands when opening your Unity project.
