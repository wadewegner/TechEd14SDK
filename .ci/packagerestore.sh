#!/bin/sh -x

mono --runtime=v4.0 .ci/NuGet.exe restore $@ -ConfigFile NuGet.Config -Source nuget -Verbosity detailed -NoCache -DisableParallelProcessing

if [ $? -ne 0 ]
then   
 exit 1
fi

exit $?