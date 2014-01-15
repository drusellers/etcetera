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

directory 'build/pkg'

desc 'package nugets - finds all projects and package them'
nugets_pack :create_nugets => ['build/pkg', :versioning, :build] do |p|
  p.files   = FileList['./**/*.{csproj,fsproj,nuspec}'].
    exclude(/specs/)

  p.out     = 'build/pkg'
  p.exe     = '.nuget/NuGet.exe'
  p.with_metadata do |m|
    m.id = "etcetera"
    m.description = 'etcd client for .Net'
    m.authors = 'Dru Sellers'
    m.version = ENV['NUGET_VERSION']
  end

  p.with_package do |p|
    p.add_file 'bin/Release/etcetera.dll', 'lib/net40'
  end
end