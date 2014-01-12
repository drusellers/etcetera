require 'bundler/setup'

require 'albacore'


build :build do |b|
	b.sln = "etcetera.sln"
end

task :nuget do
	#make dir build
	#make dir lib
	#make dir net40
	cp "etcetera/bin/Release/etcetera.dll", "build/lib/net40/"
	cp "etcetera.nuspec", "build/"
end
