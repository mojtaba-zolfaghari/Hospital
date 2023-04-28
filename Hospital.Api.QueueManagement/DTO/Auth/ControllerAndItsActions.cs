namespace Hospital.Api.QueueManagement.DTO.Auth
{
    /// <summary>
    /// ControllerAndItsActions
    /// </summary>
    public class ControllerAndItsActions
    {
        /// <summary>
        /// Controller
        /// </summary>
        public string Controller { get; }
        /// <summary>
        /// Actions
        /// </summary>
        public List<string> Actions { get; }
        /// <summary>
        /// ControllerAndItsActions
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="actions"></param>

        public ControllerAndItsActions(string controller, List<string> actions) => (Controller, Actions) = (controller, actions);
    }
}
