using Quartz;

namespace MemorizeWords.Quartz
{
    public static class JobScheduler
    {
        public static void ConfigureJobs(this WebApplicationBuilder builder)
        {
            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                SetLearnedWordToUnlearnedInOneWeekJob(q);
                UserGuessedWordsJob(q);
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        private static void UserGuessedWordsJob(IServiceCollectionQuartzConfigurator q)
        {
            // Create a "key" for the job
            var jobKey = new JobKey("UserGuessedWordsJob");

            // Register the job with the DI container
            q.AddJob<UserGuessedWordsJob>(opts => opts.WithIdentity(jobKey));

            // Create a trigger for the job
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("UserGuessedWordsJob-trigger")
                //.WithCronSchedule("0/5 * * ? * * *")); // 5 seconds
                .WithCronSchedule("* * * * * ?"));
                //.WithCronSchedule("0/30 * * ? * * *"));
        }

        private static void SetLearnedWordToUnlearnedInOneWeekJob(IServiceCollectionQuartzConfigurator q)
        {
            // Create a "key" for the job
            var jobKey = new JobKey("SetLearnedWordToUnlearnedInOneWeekJob");

            // Register the job with the DI container
            q.AddJob<SetLearnedWordToUnlearnedInOneWeekJob>(opts => opts.WithIdentity(jobKey));

            // Create a trigger for the job
            q.AddTrigger(opts => opts
                .ForJob(jobKey) // link to the SetLearnedWordToUnlearnedInOneWeekJob
                .WithIdentity("SetLearnedWordToUnlearnedInOneWeekJob-trigger") // give the trigger a unique name
                //.WithCronSchedule("0/5 * * ? * * *")); // 5 seconds
                 .WithCronSchedule("0 0 0 * * ?"));
        }
    }
}
