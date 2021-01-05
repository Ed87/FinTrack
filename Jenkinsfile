pipeline {
			agent any
			stages {
				stage('Checkout'){
					steps{
						checkout([$class: 'GitSCM', branches: [[name: '*/develop']], doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], userRemoteConfigs: [[credentialsId: 'Golide', url: 'https://github.com/Ed87/FinTrack.git']]])
					}
				}
				stage('Build') {
    					steps {
    					    bat "\"${tool 'MSBuild'}\" PaySys.sln /p:DeployOnBuild=true /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:SkipInvalidConfigurations=true /t:build /p:Configuration=Release /p:Platform=\"Any CPU\" /p:DeleteExistingFiles=True /p:publishUrl=c:\\inetpub\\wwwroot\\paymentswebapp"
    					}
				}
				stage('Quality Gate') {
   steps {
       script {
       def MSBuildScannerHome = tool 'MSBuild_SonarScanner';
           withSonarQubeEnv("LocalSonar") {
           bat "${MSBuildScannerHome}\\SonarQube.Scanner.MSBuild.exe end"
           -Dsonar.projectKey=PaySys \
           -Dsonar.sources=. \
           -Dsonar.css.node=. \
           -Dsonar.host.url=http://localhost:9000 \
           -Dsonar.login=061603e96608663d139c9009b29eae295000584c"
               }
           }
       }
   }
			}
}