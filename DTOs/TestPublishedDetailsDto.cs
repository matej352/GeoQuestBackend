namespace GeoQuest.DTOs
{
    public class TestPublishedDetailsDto
    {
        public int Id { get; set; }

        public TimeSpan Duration { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public int FinishedInstanceCount { get; set; }
        public int FinishedByStudentsInstanceCount { get; set; }

        public int InstanceCount { get; set; }

        public bool Active { get; set; }

        public int CheckedInstanceCount { get; set; }


        public TimeSpan AvgElapsedTime { get; set; }    // calculated for finished test instances


        public int TotalPoints { get; set; }  // equal to number of tasks in test instance (1 task is worth 1 point)  
        public decimal AvgPoints { get; set; } // calculated for finished and checked test instances (test instance is checked if all its tasks are checked)



        public List<TestInstanceForTeacherDto> TestInstances { get; set; }
    }


    public class TestInstanceForTeacherDto
    {
        public int Id { get; set; }
        public string Student { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public int Points { get; set; }

        public bool Finished { get; set; }

        public bool Started { get; set; }

        public bool Checked { get; set; }

    }
}
