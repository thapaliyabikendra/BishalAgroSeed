# Build Docker image
if [ -z "$1" ]
  then
    echo "***********************************************"
    echo ""
    echo "Version must be specified e.g. sh build-server.sh 1.0.0.0"
    echo ""
    echo "***********************************************"
    exit
fi

docker build -t bishal-agro-seed/server:$1 -f ../../aspnet-core/src/BishalAgroSeed.HttpApi.Host/Dockerfile ./../../aspnet-core
