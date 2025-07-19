# Build Docker image
if [ -z "$1" ]
  then
    echo "***********************************************"
    echo ""
    echo "Version must be specified e.g. sh build-ui.sh 1.0.0.0"
    echo ""
    echo "***********************************************"
    exit
fi

docker build -t bishal-agro-seed/ui:$1 -f ../../angular/Dockerfile ./../../angular