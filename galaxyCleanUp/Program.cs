using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchestrA.GRAccess;

namespace galaxyCleanUp
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            string node;
            string galaxyName;
            string galaxyUser;
            string galaxyPass;
            List<string> protectedTemplates = new List<string>();
            string line;


            Console.Write("Enter Galaxy Node name: ");
            node = Console.ReadLine();
            Console.Write("Enter Galaxy Name: ");
            galaxyName = Console.ReadLine();
            Console.Write("Enter Galaxy Username: ");
            galaxyUser = Console.ReadLine();
            Console.Write("Enter Galaxy Password: ");
            galaxyPass = Console.ReadLine();

            System.IO.StreamReader file = new System.IO.StreamReader(@"protectedObjects.txt");

            while ((line = file.ReadLine()) != null)
            {
                protectedTemplates.Add(line.ToLower());                
            }

            try
            {
                log.Info("Attempting to connect to " + galaxyName + " using a username of " + galaxyUser);
                GRAccessApp grAccess = new GRAccessAppClass();

                IGalaxies gals = grAccess.QueryGalaxies(node);
                log.Info("Found " + gals.count + " galaxies");
                IGalaxy galaxy = gals[galaxyName];
                log.Info("Attempting to log into galaxy");
                galaxy.Login(galaxyUser, galaxyPass);

                IgObjects objs = galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsTemplate, EConditionType.NameEquals, "thisshouldnevermatch", EMatch.NotMatchCondition);

                log.Info("Found " + objs.count + " objects");

                int x = 0;
                foreach (ITemplate template in objs)
                {
                    IgObjects derivedtemplates = galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsTemplate, EConditionType.derivedOrInstantiatedFrom, template.Tagname, EMatch.MatchCondition);
                    IgObjects derivedinstances = galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.derivedOrInstantiatedFrom, template.Tagname, EMatch.MatchCondition);
                    log.Info("Checking " + template.Tagname + " for derived templates");
                    if (derivedtemplates.count == 0)
                    {
                        log.Info(template.Tagname + " has no derived templates, checking instances now");
                        if (derivedinstances.count == 0)
                        {
                            if (!protectedTemplates.Contains(template.Tagname.ToLower()))
                            {
                                log.Info(template.Tagname + " has no instances, this template will be removed from the galaxy");
                                x++;
                                template.DeleteTemplate();
                            }
                            else
                            {
                                log.Info(template.Tagname + " has no instances but is a protected object so will not be deleted");
                            }
                        }
                    }
                }

                galaxy.Logout();

                Console.WriteLine("Finished. {0} objects deleted, press return to exit", x.ToString());
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
