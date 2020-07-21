using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IssueCreator.Models
{
    internal class IssueToCreate : INotifyPropertyChanged, IEquatable<IssueToCreate>
    {
        private string title;
        private string description;
        private List<string> labels;

        public string Organization { get; set; }
        public string Repository { get; set; }
        public string AssignedTo { get; set; }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                NotifyPropertyChanged();
            }
        }
        public string Description
        {
            get => description; set
            {
                description = value;
                NotifyPropertyChanged();
            }
        }
        public List<string> Labels
        {
            get => labels; set
            {
                labels = value;
                NotifyPropertyChanged();
            }
        }
        public IssueDescription Epic { get; set; }
        public IssueMilestone Milestone { get; set; }
        public string Estimate { get; set; }
        public bool CreateAsEpic { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Equals(IssueToCreate other)
        {
            if (other == null)
            {
                return false;
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(this.Organization, other.Organization) &&
                StringComparer.OrdinalIgnoreCase.Equals(this.Repository, other.Repository) &&
                StringComparer.OrdinalIgnoreCase.Equals(this.Title, other.Title) &&
                StringComparer.OrdinalIgnoreCase.Equals(this.Description, other.Description))
            {
                return true;
            }

            return false;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
