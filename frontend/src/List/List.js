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

function Infos(props) {
  const [schedule, setSchedule] = useState('');
  const { isOpen, onOpen, onClose } = useDisclosure();

  const handleClick = () => {
    onOpen();
  };
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
      handleClick();
      toast({
        title: 'Test created.',
        description: `New run executed`,
        status: 'info',
        duration: 9000,
        isClosable: true,
      });
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
    <Tr>
      <Td>{props.item.name}</Td>
      <Td>
        <Button
          onClick={() => {
            run(props.item.id);
          }}
          m={4}
        >
          Run test
        </Button>
      </Td>

      <Td>
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
      </Td>
      <Td>
        <Button
          onClick={() => handleClick()}
          m={4}
          disabled={props.item.runs.length === 0}
        >
          View runs
        </Button>
      </Td>
      <Drawer onClose={onClose} isOpen={isOpen} size="xl">
        <DrawerOverlay />
        <DrawerContent>
          <DrawerCloseButton />
          <DrawerHeader>{props.item.name} - List of runs</DrawerHeader>
          <DrawerBody>
            <Accordion>
              {props.item.runs.map(run => (
                <AccordionItem key={run.id}>
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
                    {run.state === 'Succeeded' || run.state === 'Failed' ? (
                      <video
                        controls
                        src={`http://localhost:5001/run/${run.id}/video`}
                      ></video>
                    ) : (
                      'Test in progress, please wait.'
                    )}
                  </AccordionPanel>
                </AccordionItem>
              ))}
            </Accordion>
          </DrawerBody>
        </DrawerContent>
      </Drawer>
    </Tr>
  );
}

function MyList(props) {
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
            <Infos item={item} key={item.id} />
          ))}
        </Tbody>
      </Table>
    </TableContainer>
  );
}
export default MyList;
