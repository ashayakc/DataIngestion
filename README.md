# DataIngestion

### Run instructions:
- Navigate to `DataIngestion/MusicDataIngestion`
- Run `dotnet restore` to restore the project
- Run `dotnet build` to build the project
- Run `dotnet publish -o ./publish`

To simulate the data ingestion, please download all 4 datasets from [here](https://drive.google.com/drive/folders/1uvIjNqVZRK2PuxvNI6Mh9UYJM8KMPpOv) and place it within publish folder.

- Run `docker-compose build`
- Run `docker-compose up`

This should successfully bring up Elastic node, Kibana & Music data ingestion service up and start processing the datasets automatically to elastic search.

### Run Unit tests:
 - Navigate to `DataIngestion/MusicDataIngestion_uTest`
 - Run `dotnet test` to run all unit tests

### How music data ingestion is implemented?

By taking a look at the datasets, the idea here was to load the artist, artist_collection & collection_match data in memory as they are low in size and can easily maintained in memory which adds value in terms of performance by not hitting the memory limits. The file which is comparitively larger was collection also the output collection object index to elastic had many columns to be appeared from collection dataset. 

So i decided to run through the collection dataset, pick the items in a batch of 1000 (configurable, as its a good amount to send to elastic in batch, i chose 1000), bulk process it to elastic and repeat until complete. The tool is built with keeping, performance, scalability & memory optimization in mind.
