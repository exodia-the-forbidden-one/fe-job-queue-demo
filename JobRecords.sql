create table "JobRecords"
(
    "Id"           uuid                     not null
        constraint "PK_JobRecords"
            primary key,
    "QueueID"      text                     not null,
    "ExecuteAfter" timestamp with time zone not null,
    "ExpireOn"     timestamp with time zone not null,
    "IsComplete"   boolean                  not null,
    "CommandJson"  text                     not null
);