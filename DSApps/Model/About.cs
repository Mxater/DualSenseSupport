using System;
using JetBrains.Annotations;
using ReactiveUI;
namespace DSApps.Model
{
    public class About : ReactiveObject
    {
        private string versionName;
        public string VersionName
        {
            get => versionName;
            set => this.RaiseAndSetIfChanged(ref versionName, value);
        }

        private bool isNewUpdate = false;
        public bool IsNewUpdate
        {
            get => isNewUpdate;
            set => this.RaiseAndSetIfChanged(ref isNewUpdate, value);
        }

        private string actualVersion;
        public string ActualVersion
        {
            get => actualVersion;
            set => this.RaiseAndSetIfChanged(ref actualVersion, value);
        }
        
        private string versionUrl;
        public string VersionUrl
        {
            get => versionUrl;
            set => this.RaiseAndSetIfChanged(ref versionUrl, value);
        }
        
        
        
    }
}