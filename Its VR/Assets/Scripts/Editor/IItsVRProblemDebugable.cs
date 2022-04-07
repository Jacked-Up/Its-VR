namespace ItsVR.Editor {
    /// <summary>
    /// It's VR problem debugger interface. Used for refreshing It's VR component problems.
    ///
    /// 'RefreshProblems' method is invoked everytime a refresh is requested by the developer.
    /// </summary>
    /// <para>Author: Jack Randolph</para>
    public interface IItsVRProblemDebugable {
        void RefreshProblems();
    }
}
