pipeline {
    agent any

    stages {
        stage('Test Command') {
            steps {
                sh 'echo "Hello, Jenkins!"'
            }
        }

      stage('Run and leave') {
        steps {
          sh 'docker-compose up --build'
        }
    }
}
