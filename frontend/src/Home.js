import { Text } from '@chakra-ui/react';
import homeSrc from './home.jpg';

function Home() {
  return (
    <div style={{ position: 'relative' }}>
      <img src={homeSrc} style={{ filter: 'brightness(0.7)' }} />
      <div
        style={{
          bottom: '20%',
          position: 'absolute',
          left: 0,
          right: 0,
          width: '100%',
        }}
      >
        <Text fontSize="6xl" color="white">
          <b>Testing has never been easier!</b>
        </Text>
      </div>
    </div>
  );
}

export default Home;
