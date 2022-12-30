import { Link } from "react-router-dom";
import { Card, Image } from "semantic-ui-react";
import { ProfileEvent } from "../../app/models/profile";
import { format } from "date-fns";
import styles from "./ProfileEventCard.module.css";

interface Props {
	event: ProfileEvent;
}

export default function ProfileEventCard({ event }: Props) {
	return (
		<Card as={Link} to={`/activities/${event.id}`}>
			<Image
				wrapped
				ui={false}
				src={`/assets/categoryImages/${event.category}.jpg`}
				className={styles.image}
			/>
			<Card.Content textAlign="center">
				<Card.Header>{event.title}</Card.Header>
				<Card.Description>
					<div>{format(new Date(event.date), "dd MMM")}</div>
					<div>{format(new Date(event.date), "h:mm aa")}</div>
				</Card.Description>
			</Card.Content>
		</Card>
	);
}
