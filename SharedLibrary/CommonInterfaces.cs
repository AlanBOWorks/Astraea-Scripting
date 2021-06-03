using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibrary
{
    /// <summary>
    /// Used to emulate Transform but in a more performance and organized matter (instead of referencing
    /// all types of transform everywhere and getting those data from anywhere is given by
    /// this interface.
    /// <br></br><br></br>
    /// - TLDR: it's just for organization purposes
    /// </summary>
    public interface ICharacterTransformData
    {
        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        Vector3 MeshWorldPosition { get; }
        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        Vector3 MeshForward { get; }
        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        Vector3 MeshUp { get; }
        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        Vector3 MeshRight { get; }

        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        Transform Root { get; }
        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        Transform MeshRoot { get; }

        [ShowInInspector, HideInEditorMode, DisableInPlayMode]
        Transform Head { get; }
        [ShowInInspector, HideInEditorMode, DisableInPlayMode]
        Vector3 HeadPosition { get; }

        [ShowInInspector, HideInEditorMode, DisableInPlayMode]
        Transform Pelvis { get; }

        Vector3 PelvisPosition { get; }

    }

    public interface ITicker
    {
        /// <summary>
        /// Used by <seealso cref="TickerHandler"/> to ignore the <see cref="Tick"/> instruction
        /// </summary>
        bool Disabled { get; set; }

        void Tick();
    }
}