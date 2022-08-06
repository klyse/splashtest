import {
  Drawer,
  DrawerBody,
  DrawerHeader,
  DrawerOverlay,
  DrawerContent,
  DrawerCloseButton,
  useDisclosure,
  Button,
  Accordion,
  Badge,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  AccordionIcon,
  TableContainer,
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  FormControl,
  FormLabel,
  Select,
  Box,
} from '@chakra-ui/react';
import { useToast } from '@chakra-ui/react';
import { useState } from 'react';

function MySelector() {
  const [schedule, setSchedule] = useState('');
  return (
    <FormControl>
      <Select
        placeholder=""
        value={schedule}
        onChange={e => setSchedule(e.target.value)}
      >
        <option value=""></option>
        <option value="option1">every 30 min</option>
        <option value="option2">every 6 hours</option>
        <option value="option3">every day</option>
      </Select>
    </FormControl>
  );
}

function MoreInfo(props) {
  const { isOpen, onOpen, onClose } = useDisclosure();

  const handleClick = () => {
    onOpen();
  };

  return (
    <>
      <Button
        onClick={() => handleClick()}
        m={4}
        disabled={props.item.runs.length === 0}
      >
        View runs
      </Button>

      <Drawer onClose={onClose} isOpen={isOpen} size="xl">
        <DrawerOverlay />
        <DrawerContent>
          <DrawerCloseButton />
          <DrawerHeader>{props.item.name} - List of runs</DrawerHeader>
          <DrawerBody>
            <Accordion>
              {props.item.runs.map(run => (
                <AccordionItem>
                  <AccordionButton>
                    <Box flex="1" textAlign="left">
                      <Badge
                        mr={4}
                        colorScheme={
                          run.state === 'Succeeded'
                            ? 'green'
                            : run.state === 'Failed'
                            ? 'red'
                            : 'yellow'
                        }
                      >
                        {/* pending */}
                        {run.state}
                      </Badge>
                      {run.runDateTime}
                    </Box>
                    <AccordionIcon />
                  </AccordionButton>
                  <AccordionPanel pb={4}>
                    <video
                      controls
                      src={`http://localhost:5001/run/${run.id}/video`}
                    ></video>
                  </AccordionPanel>
                </AccordionItem>
              ))}
            </Accordion>
          </DrawerBody>
        </DrawerContent>
      </Drawer>
    </>
  );
}

function MyList(props) {
  const toast = useToast();
  const run = async id => {
    try {
      const response = await fetch(`http://localhost:5001/test/${id}/run`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Accept: 'application/json',
        },
      });
      toast({
        title: 'Test created.',
        description: `New run executed`,
        status: 'info',
        duration: 9000,
        isClosable: true,
      });
      // setTitle('');
      // setAuthor('');
      // setEmail('');
      // setInterval('');
      // // setSchedule('');
      // setItem([]);
    } catch (e) {
      toast({
        title: 'Error.',
        description: `Cannot create run`,
        status: 'error',
        duration: 9000,
        isClosable: true,
      });
    }
  };
  return (
    <TableContainer>
      <Table variant="simple">
        <Thead>
          <Tr>
            <Th>Test</Th>
            <Th></Th>
            <Th>Schedule</Th>
            <Th></Th>
          </Tr>
        </Thead>
        <Tbody>
          {props?.items?.map(item => (
            <Tr>
              <Td>{item.name}</Td>
              <Td>
                <Button
                  onClick={() => {
                    run(item.id);
                  }}
                  m={4}
                >
                  Run test
                </Button>
              </Td>
              <Td>
                <MySelector />
              </Td>
              <Td>
                <MoreInfo item={item} />
              </Td>
            </Tr>
          ))}
        </Tbody>
      </Table>
    </TableContainer>
  );
}

export default MyList;
