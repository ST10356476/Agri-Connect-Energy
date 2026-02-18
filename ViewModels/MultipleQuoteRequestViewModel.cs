// ViewModels/MultipleQuoteRequestViewModel.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri_Energy_Connect.Models;


namespace Agri_Energy_Connect.ViewModels
{
    /// <summary>
    /// View model for requesting quotes for multiple solutions
    /// </summary>
    public class MultipleQuoteRequestViewModel
    {
        /// <summary>
        /// List of solution IDs for quote request
        /// </summary>
        public List<int> SolutionIds { get; set; } = new List<int>();

        /// <summary>
        /// List of solution details
        /// </summary>
        public List<EnergySolution> Solutions { get; set; } = new List<EnergySolution>();

        /// <summary>
        /// Quote request information
        /// </summary>
        public QuoteRequestModel QuoteRequest { get; set; } = new QuoteRequestModel();

        /// <summary>
        /// Additional message for multiple quote request
        /// </summary>
        [Display(Name = "Additional Message")]
        public string AdditionalMessage { get; set; }
    }
}