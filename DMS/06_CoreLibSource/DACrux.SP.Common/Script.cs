using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DACrux.SP.Common
{
    [Serializable]
    public class Script
    {
        #region " Member Field "

        private Data[] args = null;
        private string targetTask = string.Empty;

        #endregion

        #region " Property "

        public Data[] Args
        {
            get { return args; }
            set { args = value; }
        }

        public ITask TargetTask
        {
            get { return Analysis.GetInstance().Tasks[targetTask]; }
            set { targetTask = value.Name; }
        }

        public TaskRunningModeItem RunningMode { get; set; }

        public ScriptCommandTypeItem CommandType { get; set; }

        #endregion

        #region " Creator "

        public Script() { }

        public Script(ScriptCommandTypeItem commandType, TaskRunningModeItem requestType, ITask targetTask, params Data[] args)
        {
            this.CommandType = commandType;
            this.RunningMode = requestType;
            this.TargetTask = targetTask;
            this.args = args;
        }

        #endregion
    }

    public class ScriptRunner
    {
        #region " Member Field "

        ITaskCollection tasks;
        List<Script> scripts;

        ILogger logger;

        #endregion

        #region " Creator "

        private ScriptRunner() { }

        public ScriptRunner(ITaskCollection tasks, List<Script> scripts, ILogger logger)
        {
            this.tasks = tasks;
            this.scripts = scripts;

            this.logger = logger;
        }

        #endregion

        #region " Method "

        public void Run()
        {
            try
            {
                logger.WriteLog(LogTypeItem.Notice, "Script Excution Started.");

                if (!CheckGrammar(logger, scripts))
                {
                    return;
                }

                foreach (Script script in scripts)
                    RunScript(script);

                logger.WriteLog(LogTypeItem.Notice, "Script Excution Completed.");
            }
            catch (Exception ex)
            {
                logger.WriteLog(LogTypeItem.Error, ex.Message);
            }
        }

        public static bool CheckGrammar(ILogger logger, List<Script> scripts)
        {
            bool isValidGrammar = true;

            try
            {
                logger.WriteLog(LogTypeItem.Notice, "Script grammar checking started.");

                Stack stack = new Stack();

                foreach (Script script in scripts)
                {
                    logger.WriteLog(LogTypeItem.Notice, string.Format("Checking script : {0} {1}", script.CommandType.ToString(), string.Join(" ", (from Data data in script.Args select data.Value).ToArray())));

                    switch (script.CommandType)
                    {
                        case ScriptCommandTypeItem.REPEAT_START:
                            if (script.TargetTask == null)
                            {
                                logger.WriteLog(LogTypeItem.Error, "No Repeator Task specified.");
                                isValidGrammar = false;
                            }
                            else
                            {
                                stack.Push(script.TargetTask.Name);
                            }
                            break;
                        case ScriptCommandTypeItem.REPEAT_END:
                            if (script.TargetTask == null)
                            {
                                logger.WriteLog(LogTypeItem.Error, "No Repeator Task specified.");
                                isValidGrammar = false;
                            }
                            else
                            {
                                if (((string)stack.Pop()) != script.TargetTask.Name)
                                {
                                    logger.WriteLog(LogTypeItem.Error, "Repeator START / END pair is not matched.");
                                    isValidGrammar = false;
                                }
                            }
                            break;
                        case ScriptCommandTypeItem.RUN:
                            if (script.TargetTask == null)
                            {
                                logger.WriteLog(LogTypeItem.Error, "No Target Task specified.");
                                isValidGrammar = false;
                            }
                            break;
                        case ScriptCommandTypeItem.SET_VALUE:
                            if (script.TargetTask == null)
                            {
                                logger.WriteLog(LogTypeItem.Error, "No Target Task and Value specified.");
                                isValidGrammar = false;
                            }
                            else if (script.Args.Length < 1)
                            {
                                logger.WriteLog(LogTypeItem.Error, "No Value specified.");
                                isValidGrammar = false;
                            }
                            break;
                        case ScriptCommandTypeItem.SET_ARGS:
                            if (script.TargetTask == null)
                            {
                                logger.WriteLog(LogTypeItem.Error, "No Target Task and Value specified.");
                                isValidGrammar = false;
                            }
                            else if (script.Args.Length < 1)
                            {
                                logger.WriteLog(LogTypeItem.Error, "No Value specified.");
                                isValidGrammar = false;
                            }
                            break;
                        default:
                            break;
                    }
                }

                logger.WriteLog(LogTypeItem.Notice, "Script grammar checking completed.");

                return isValidGrammar;
            }
            catch (Exception ex)
            {
                logger.WriteLog(LogTypeItem.Error, ex.Message);
                return false;
            }
        }

        private void RunScript(Script script)
        {
            try
            {
                logger.WriteLog(LogTypeItem.Notice, string.Format("{0} {1} {2} {3}", script.CommandType.ToString(), script.TargetTask.Name, script.RunningMode.ToString(), string.Join(",", (from Data data in script.Args select data.Value).ToArray())));

                switch (script.CommandType)
                {
                    case ScriptCommandTypeItem.REPEAT_START:
                        foreach (Data data in script.TargetTask)
                        {
                            int startIndex = scripts.IndexOf(script);
                            int endIndex = scripts.FindIndex(startIndex, sc => sc.CommandType == ScriptCommandTypeItem.REPEAT_END);

                            if (endIndex < 0) break;

                            for (int i = startIndex+1; i < endIndex; i++)
                                RunScript(scripts[i]);
                        }
                        break;
                    case ScriptCommandTypeItem.REPEAT_END:
                        break;
                    case ScriptCommandTypeItem.RUN:
                        script.TargetTask.Excute(script.RunningMode);
                        break;
                    case ScriptCommandTypeItem.SET_VALUE:
                        script.TargetTask.Result = script.Args[1].Value;
                        break;
                    case ScriptCommandTypeItem.SET_ARGS:
                        script.TargetTask.Arguments = script.Args.ToList();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog(LogTypeItem.Error, ex.Message);
            }
        }

        #endregion
    }
}
