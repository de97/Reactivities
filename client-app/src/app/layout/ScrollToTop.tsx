import {useEffect} from "react";
import {useLocation} from "react-router-dom";

export default function ScrollToTop() {
    const { pathname } = useLocation();
   
    useEffect(() => {
      window.scrollTo(0, 0);
    }, [pathname]);
   
    return null;
}

 
  


// extends React.Component {
//   componentDidUpdate(prevProps) {
//     if (
//       this.props.location.pathname !== prevProps.location.pathname
//     ) {
//       window.scrollTo(0, 0);
//     }
//   }

//   render() {
//     return null;
//   }
// }