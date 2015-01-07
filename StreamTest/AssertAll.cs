using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StreamTests
{
    /*
     * Adapted from: http://blog.drorhelper.com/2011/02/multiple-asserts-done-right.html
     * Inspired by: http://osherove.com/blog/2011/2/9/multiple-mocks-asserts-and-hidden-results.html
     * NUnit version: http://rauchy.net/oapt/
     */
    public class AssertAll
    {
        public static void Execute(params Action[] assertionsToRun)
        {
            var errors = new List<string>();
            foreach (var action in assertionsToRun)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception exc)
                {
                    errors.Add(exc.Message);
                }
            }

            if (errors.Any())
            {
                string errorMessageString = string.Join("|", errors.ToArray());

                Assert.Fail(string.Format("{0}/{1} conditions failed:{2}{3}",
                                          errors.Count, assertionsToRun.Count(), 
                                          " ",
                                          errorMessageString));
            }
        }
    }
}
