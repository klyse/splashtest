import {
  Box,
  Button,
  Flex,
  FormControl,
  HStack,
  Input,
  Select,
  Stack,
  Text,
  Textarea,
} from '@chakra-ui/react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useEffect, useState } from 'react';

export const LoadTest = () => {
  const [connection, setConnection] = useState(null);
  const [loadTestResult, setLoadTestResult] = useState([]);
  const [isRunning, setIsRunning] = useState(false);

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
      setLoadTestResult(prev => [...prev, progress.message]);
    });

    setConnection(_connection);
  }, []);

  return (
    <Flex minH="100vh">
      <Stack width="100%">
        <HStack>
          <Input value="https://hackathon.bz.it" />
          <FormControl>
            <Select>
              <option value="option1">every 8 hours</option>
              <option value="option3">every day</option>
              <option value="option3">every second day</option>
            </Select>
          </FormControl>
          <FormControl>
            <Select>
              <option value="option1">20 parallel requests</option>
              <option value="option2">50 parallel requests</option>
              <option value="option3">100 parallel requests</option>
            </Select>
          </FormControl>
          <Text>Strategy:</Text>
          <FormControl>
            <Select>
              <option value="option1">Warm Up and Cool Down</option>
              <option value="option2">Full</option>
              <option value="option3">Warm Up</option>
            </Select>
          </FormControl>
        </HStack>
        <Button bg="green.400" disabled={isRunning}
          onClick={() => {
            setLoadTestResult([]);
            setIsRunning(true);
            connection.invoke('Run', 'test');
          }}
        >
          Run
        </Button>
        <Textarea
          value={loadTestResult.join('\n')}
          bg="gray.600"
          textColor="white"
          minH="80vh"
          lineHeight={1}
        />
      </Stack>
    </Flex>
  );
};
