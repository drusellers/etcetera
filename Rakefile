require 'bundler/setup'

require 'albacore'
require 'albacore/tasks/versionizer'


Albacore::Tasks::Versionizer.new :versioning

task :default => :build

desc 'build project'
build :build do |b|
	b.sln = "etcetera.sln"
end

desc 'Perform full build'
build :build => [:versioning, :restore] do |b|
  b.sln = 'etcetera.sln'
end

desc 'restore all nugets as per the packages.config files'
nugets_restore :restore do |p|
  p.out = 'packages'
  p.exe = '.nuget/NuGet.exe'
end

FileUtils.mkdir_p 'build/pkg/lib/net40'

desc 'package nugets - finds all projects and package them'
task :nuget do
  cp 'etcetera/bin/Release/etcetera.dll', 'build/pkg/lib/net40'
  sh '.nuget/nuget.exe pack etcetera.nuspec -BasePath build/pkg -Output build'
end

task :release do
  sh '.nuget/nuget.exe push build/etcetera.0.5.0.nupkg'
end
