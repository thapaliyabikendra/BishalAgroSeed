# Build Docker image
if [ -z "$1" ]
  then
    echo "***********************************************"
    echo ""
    echo "Version must be specified e.g. sh build-client.sh 1.0.0.0"
    echo ""
    echo "***********************************************"
    exit
fi

docker build -t bishal-agro-seed/client:$1 -f ../../angular/Dockerfile ./../../angular