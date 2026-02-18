// ViewModels/QuoteConfirmationViewModel.cs
using Agri_Energy_Connect.ViewModels;


namespace Agri_Energy_Connect.ViewModels
{
    /// <summary>
    /// View model for quote confirmation page
    /// </summary>
    public class QuoteConfirmationViewModel
    {
        /// <summary>
        /// The submitted quote request details
        /// </summary>
        public QuoteRequestModel QuoteRequest { get; set; }

        /// <summary>
        /// Quote request ID for tracking
        /// </summary>
        public int QuoteRequestId { get; set; }

        /// <summary>
        /// Confirmation number for the quote request
        /// </summary>
        public string ConfirmationNumber { get; set; }

        /// <summary>
        /// Expected contact time from providers
        /// </summary>
        public string ExpectedContactTime { get; set; } = "within 24 hours";

        /// <summary>
        /// Next steps information
        /// </summary>
        public List<string> NextSteps { get; set; } = new List<string>();
    }
}