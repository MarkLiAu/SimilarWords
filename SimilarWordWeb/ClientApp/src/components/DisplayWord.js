import React, { useState } from 'react';
import { Button, Panel } from 'react-bootstrap';

const DisplayWord = ({ word }) => {
    // Declare a new state variable, which we'll call "count"
    const [count, setCount] = useState(0);
            //<p>You clicked {count} times</p>
            //<button onClick={() => setCount(count + 1)}>
            //    Click me
            //</button>

    return (

        <Panel>
            {word.name}
        </Panel>


    );
}

export default DisplayWord;
