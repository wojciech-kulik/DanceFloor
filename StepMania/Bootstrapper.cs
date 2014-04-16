using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using StepMania.ViewModels;
using Caliburn.Micro;
using System.Reflection;
using Common;
using ApplicationServices;

namespace StepMania
{
    public class Bootstrapper : Bootstrapper<MainWindowViewModel>
    {
        private SimpleContainer container;

        protected override void Configure()
        {
            container = new SimpleContainer();
            container.PerRequest<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            
            container.PerRequest<MainWindowViewModel>();
            container.PerRequest<MenuViewModel>();
            container.PerRequest<GameViewModel>();

            container.Singleton<ISongsService, SongsService>();
            container.PerRequest<IMusicPlayerService, MusicPlayerService>();
            container.PerRequest<ISequenceCreationService, SequenceCreationService>();
            container.PerRequest<IHighScoresService, HighScoresService>();
            container.Singleton<ISettingsService, SettingsService>();
            
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}