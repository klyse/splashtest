import { Box, Button, Flex, Text } from '@chakra-ui/react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useEffect, useRef, useState } from 'react';

export const LoadTest = () => {
  const [connection, setConnection] = useState(null);
  const [loadTestResult, setLoadTestResult] = useState([]);

  useEffect(() => {
    const _connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5001/load-test')
      .configureLogging(LogLevel.Information)
      .build();

    async function start() {
      try {
        await _connection.start();
        console.log('SignalR Connected.');
      } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
      }
    }

    start();

    _connection.on('progress', progress => {
        console.log("progress");
      setLoadTestResult(prev => [...prev, progress.Message]);
    });

    setConnection(_connection);
  }, []);

  return (
    <Flex>
      <Button
        onClick={() => {
          connection.invoke('Run', 'test');
        }}
      >
        Run
      </Button>
      <Box>
        {loadTestResult.map(d => {
          return <Text>{d}</Text>;
        })}
      </Box>
    </Flex>
  );
};
