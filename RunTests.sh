#!/bin/sh -x

# package restore
.ci/packagerestore.sh src/Salesforce.Force.sln

# build projects
xbuild src/Salesforce.Force.sln

# unit tests
.ci/nunit.sh src/Salesforce.Force.FunctionalTests/bin/Debug/Salesforce.Force.FunctionalTests.dll
.ci/nunit.sh src/Salesforce.Force.UnitTests/bin/Debug/Salesforce.Force.UnitTests.dll