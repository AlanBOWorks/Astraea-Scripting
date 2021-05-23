using System.Collections.Generic;

using MEC;
using UnityEngine;

namespace AIEssentials
{
    public interface IPathCalculator
    {
        IEnumerator<float> RefreshSearch(Vector3 targetPoint);
        void SetDestination(Vector3 targetPoint);
        Vector3 GetDestination();

        Vector3 SteeringPoint();
        Vector3 DesiredVelocity();
        float GetRemainingDistance();
        bool IsCloseEnough(float distanceThreshold);

        void SetReachDestinationDistance(float distance);
        bool HasReachedDestination();
        bool HasPath();
    }

    public interface IRequestOnEmpty<out T>
    {
        T OnEmptyRequest();
    }
    public interface IEnumeratorOnEmpty : IRequestOnEmpty<IEnumerator<float>>{}

    /// <summary>
    /// It should only contains one Coroutine because it can become quite messy once
    /// there's a lot of <see cref="IAction"/>s at once.
    /// <br></br>
    /// It should represent a basic and straight forward action than should be
    /// easy to track in mind <example>(eg: go To Point, stay in formation, pick up an object, etc)</example>.
    /// </summary>
    public interface IAction
    {
    }

    /// <summary>
    /// This interface is used for those <seealso cref="IAction"/> that can't be stop directly
    /// and instead requires an additional step for stopping <example>(eg: returning an object,
    /// breaking a secondary coroutine, returning a <seealso cref="GameObject"/>'s parent to null...)</example>
    /// </summary>
    public interface IPreparedAction : IAction
    {
        void AuxiliaryCancel();
    }

    public interface ISequenceTask
    {
        int TaskPriority { get; }
    }

    /// <summary>
    /// It should represent a Task that won't be invoked until its condition is met. <br></br>
    /// Useful for guarding another <seealso cref="ISequenceTask"/> until this condition is met
    /// and pause that said <seealso cref="ISequenceTask"/> until
    /// this <see cref="IGuardTask"/> is finish.<br></br>
    /// An <see cref="IGuardTask"/> should pause the <see cref="ISequenceTask"/> inside
    /// the Task itself and not in the <seealso cref="IRequestManagerSingle{TRequest}"/>,
    /// which can be paused by another <seealso cref="ISequenceTask"/> with special priorities and/or special
    /// behaviour that requires to stop the Current Task;<br></br><br></br>
    /// In other words <see cref="IGuardTask"/>
    /// is mean to pause for something related to the <see cref="ISequenceTask"/> and <seealso cref="IRequestManagerSingle{TRequest}"/>
    /// pauses something non related/unknown to the <seealso cref="ISequenceTask"/> 
    /// </summary>
    public interface IGuardTask
    {}

    public interface ITaskManager : IRequestManager<CoroutineHandle,IEnumerator<float>>
    {}

    public interface IRequestManager<in TPause, TRequest> : IRequestManager<TRequest>
    {
        void StopCurrentAndResume();
        bool ContainsInPaused(dynamic referenceKey);
    }

    public interface IRequestManager<TRequest> : IRequestManagerSingle<TRequest>
    {
        void DirectEnqueue(TRequest request, int priorityLevel, dynamic requestKey);
        bool ContainsInQueue(dynamic referenceKey);
        void MoveNext();
    }

    public interface IRequestManagerEnumerator<out THandler> : IRequestManagerEnumerator
    {
        THandler CurrentHandle { get; }
    }
    public interface IRequestManagerEnumerator : IRequestManagerSingle<IEnumerator<float>>{}

    public interface IRequestManagerSingle<TRequest>
    { 
        dynamic CurrentReferenceKey { get; }
        TRequest CurrentRequest { get; }


        /// <param name="requestKey">Is to keep a reference to compare and avoid repetition on task
        /// that can't be repeated</param>
        void RequestTask(TRequest request, int priorityLevel, dynamic requestKey);
        void StopCurrentAndDoTask(TRequest request, int priorityLevel, dynamic referenceKey);
        void PauseCurrentAndDoTask(TRequest request, int priorityLevel, dynamic referenceKey);


        bool HasRequest();
        bool HasPaused();
        bool IsEmpty();
    }
}
