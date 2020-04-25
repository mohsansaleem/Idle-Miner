using System;
using System.Collections.Generic;
using PG.Core.Scenes.Popup;
using RSG;
using Zenject;

namespace PG.Core.Commands
{
    // If you plan on passing in params to the command that you want BaseCommand to manage,
    // use this interface.
    public class BaseCommandParams
    {
        public List<IBasePromise> commandPromises { get; private set; }

        public void ReserveCommandPromises(int reserveCount)
        {
            if (commandPromises == null)
            {
                commandPromises = new List<IBasePromise>(reserveCount);
            }
            else
            {
                int newCap = commandPromises.Count + reserveCount;
                if (newCap > commandPromises.Capacity)
                {
                    commandPromises.Capacity = newCap;
                }
            }
        }

        public void RegisterCommandPromise(IBasePromise promise)
        {
            if (commandPromises == null)
            {
                commandPromises = new List<IBasePromise>();
            }
            commandPromises.Add(promise);
        }

        public void RegisterCommandPromises(IBasePromise[] promises)
        {
            if (commandPromises == null)
            {
                commandPromises = new List<IBasePromise>(promises.Length);
            }
            foreach (IBasePromise promise in promises)
            {
                commandPromises.Add(promise);
            }
        }

        public void RegisterCommandPromises(List<IBasePromise> promises)
        {
            if (commandPromises == null)
            {
                commandPromises = promises;
            }
            else
            {
                foreach (IBasePromise promise in promises)
                {
                    commandPromises.Add(promise);
                }
            }
        }
    }

    public class BaseCommand
    {
        [Inject] protected OpenPopupSignal _openPopupSignal;

        public virtual void Execute()
        {
            throw new Exception("BaseCommand - Execute() is not implemented.");
        }
    }
}