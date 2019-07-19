using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfiguration
    {
        private const int EXE_EXTENSION = 0;

        private const int DLL_EXTENSION = 1;

        private const int ANYWHERE_EXTENSION = 2;

        private static readonly string[] extensions =
        {
            ".exe",
            ".dll",
            ".anywhere"
        };

        private readonly IAnyWhereConfigurationAdapterArguments adapterArguments;

        private readonly IAnyWhereConfigurationAdapterProbingPaths adapterProbingPaths;

        public AnyWhereConfiguration(
            IAnyWhereConfigurationAdapterArguments adapterArguments,
            IAnyWhereConfigurationAdapterProbingPaths adapterProbingPaths)
        {
            this.adapterArguments = adapterArguments ?? throw new ArgumentNullException(nameof(adapterArguments));
            this.adapterProbingPaths = adapterProbingPaths ?? throw new ArgumentNullException(nameof(adapterProbingPaths));
        }

        protected virtual IAnyWhereConfigurationFileSearch GetSearch()
        {
            return new AnyWhereConfigurationFileSearch(new AnyWhereConfigurationFileSystem());
        }

        protected virtual IAnyWhereConfigurationTypeSystem GetTypeSystem()
        {
            return new AnyWhereConfigurationTypeSystem(
                new AnyWhereConfigurationFileSearch(
                    new AnyWhereConfigurationFileSystem()));
        }

        public void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder)
        {
            if (configurationBuilder is null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            var search = this.GetSearch();
            if (search is null)
            {
                throw new InvalidOperationException($"An instance of {nameof(IAnyWhereConfigurationFileSearch)} is expected but found null.");
            }

            var system= this.GetTypeSystem();
            if (system is null)
            {
                throw new InvalidOperationException($"An instance of {nameof(IAnyWhereConfigurationTypeSystem)} is expected but found null.");
            }

            var directories = new HashSet<string>();
            foreach (var path in this.adapterProbingPaths.Enumerate())
            {
                directories.Add(path.Path);
            }

            if (directories.Count == 0)
            {
                return;
            }

            foreach (var arg in this.adapterArguments.Enumerate())
            {
                var results = search.Find(directories, arg.Definition.AssemblyName, extensions);
                if (results is null || results.Count == 0)
                {
                    throw new InvalidOperationException(
                        AnyWhereConfigurationExceptions.EmptySearchResultsMessage(
                            arg.Definition.AssemblyName, 
                            directories));
                }
                if (results.Count > 1)
                {
                    throw new InvalidOperationException(
                        AnyWhereConfigurationExceptions.AmbiguousSearchResultsMessage(results));
                }

                var assembly = results[0].Files[EXE_EXTENSION] ?? results[0].Files[DLL_EXTENSION];
                var config = results[0].Files[ANYWHERE_EXTENSION];

                var environment = arg.Environment;

                var content = config?.GetContentAsString();
                if (content != null)
                {
                    var values = new Dictionary<string, string>();
                    try
                    {
                        foreach (var kv in new AnyWhereConfigurationDataKeyValueEnumerable(content))
                        {
                            values[kv.Key] = kv.Value;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException(
                            AnyWhereConfigurationExceptions.ErrorLoadingEnvironmentConfigurationMessage(config.Path),
                            e);
                    }

                    environment = new AnyWhereConfigurationEnvironmentWithExtension(
                        environment,
                        new AnyWhereConfigurationEnvironment(
                            new AnyWhereConfigurationEnvironmentFromMemory(values)));
                }

                try
                {
                    var instance = (IAnyWhereConfigurationAdapter) system.Get(assembly, arg.Definition.TypeName).CreateInstance();
                    instance.ConfigureAppConfiguration(
                        configurationBuilder,
                        new AnyWhereConfigurationEnvironmentReader(environment));
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(
                        AnyWhereConfigurationExceptions.ErrorLoadingConfigurationMessage(arg.Definition),
                        e);
                }
            }
        }
    }
}