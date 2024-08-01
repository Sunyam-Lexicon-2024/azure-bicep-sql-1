#! /usr/bin/zsh

APIM_NAME="apiserviceaiw4ucnjtkvsu"
API_NAME="demo-api"
DEF_FILE_PATH="new_open_api_def.json"
OPENAPI_DEF_URL=$(echo $(az apim api export --ef OpenApiJsonUrl -n $APIM_NAME --api-id $API_NAME | jq '.properties.value.link' | sed 's/ /%20/g' | sed 's/"//g'))
OPENAPI_DEF=$(curl $OPENAPI_DEF_URL)
FUNCAPP="fnappaiw4ucnjtkvsu"
FUNCTIONS=$(echo $(az functionapp function list -n $FUNCAPP --query "[?config.bindings[?type == 'httpTrigger']].{name: name, config: config}") | jq -r)
FUNCNAME=$(echo $FUNCTIONS | jq '.[].name' | sed 's/^.*\///' | tr -d '", ')
FUNCARRAY=($(echo $FUNCTIONS | jq -r '.[]' | tr -d '\n '))

for f in $FUNCARRAY; do
    METHOD_ARRAY=()
    # Get only functions with defined methods
    METHODS=($(echo $f | jq -r '.config.bindings[] | select( .methods != null) | .methods[]'))
    # Create one path per method
    JSON=$(jq --null-input --arg funcName "/$FUNCNAME" '.[$funcName]={}')
    for m in $METHODS; do
        # Summary
        JSON=$(echo $JSON | jq --arg summary $FUNCNAME --arg parentKey "/$FUNCNAME" --arg key $m '.[$parentKey][$key].summary=$summary')
        # Operation ID
        OPERATION_ID="$m-$(echo $FUNCNAME | tr '_' '-' | tr '[:upper:]' '[:lower:]')"
        JSON=$(echo $JSON | jq --arg operationId $OPERATION_ID --arg parentKey "/$FUNCNAME" --arg key $m '.[$parentKey][$key].operationId=$operationId')
        # responses
        JSON=$(echo $JSON | jq --arg parentKey "/$FUNCNAME" --arg key $m --argjson response "{\"200\":{\"description\":null}}" '.[$parentKey][$key].responses |= $response')
    done
    echo $OPENAPI_DEF | jq --argjson paths $json '.paths += $paths' >$DEF_FILE_PATH
done
