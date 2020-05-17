import React, { Component, useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

 function ExampleHook() {
    // Declare a new state variable, which we'll call "count"
     const [count, setCount] = useState(0);
     useEffect(() => {
         // Update the document title using the browser API
         document.title = `You clicked ${count} times`;
     });
     console.log('ExampleHook' + count);
    return (
        <div>
            <p>You clicked {count} times</p>
            <button onClick={() => setCount(count + 1)}>
                Click me
            </button>
        </div>
    );
}

export function NotFound() {
    return (
        <div>
            <h2> Sorry! the page is not found! please click the link below to go back to home page</h2>
            <label> this is {React.version}</label>
            <Link to={'/'}>Home</Link>
            <ExampleHook></ExampleHook>
        </div>
    );

}

export class NotFoundZZ extends Component {
    displayName = NotFoundZZ.name

  render() {
    return (
      <div>
            <h2> Sorry, the page is not found</h2>
            <Link to={'/'}>Home</Link>

      </div>
    );
  }
}
