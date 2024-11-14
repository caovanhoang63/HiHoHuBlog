curl -u caovanhoang:Hh12032004@ -X PUT "https://localhost:9200/_ingest/pipeline/timestamp_pipeline" -H "Content-Type: application/json" -k -d'
{
  "description": "Pipeline to add/update last_modified timestamp",
  "processors": [
    {
      "set": {
        "field": "last_modified",
        "value": "{{_ingest.timestamp}}"
      }
    }
  ]
}'
